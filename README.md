autofac-extensions
==================
Some nice helpers for when working with the great .NET IoC container Autofac.

## Configure registered modules via the command line

Allows you to load modules and set module properties via command line arguments. Example:

```bash
$>  MyApp.exe -l:SomeModule,MyAssembly:Foo=Bar -s:SomeModule:Baz=42 -s:AnotherModule:Disabled
```

Detailed documentation to follow soon. The gist of it is that for loading modules via the command line, you can use the CommandLineSettingsReader like so

```csharp
var cb = new ContainerBuilder();
cb.RegisterModule(new CommandLineSettingsReader());
IContainer c = cb.Build();

using (ILifetimeScope l = c.BeginLifetimeScope())
{ 
    // resolve stuff, etc
}
```

At the moment this only support loading of modules with simple properties, I will expand it soon to provide everything you can configure in XML should you want to do so.
To load a module with or without properties, the default flag is "load:" or alternatively "l:".


For modules to be command line configurable you have to derive your module from CommandLineAwareModule instead of the vanilla Autofac Module. It's not great, but does the job right now, but I'm still working on it.

```csharp
[Alias("m")]
public class MyModule: CommandLineAwareModule
{
    [Alias("nr")]
    public int SomeNumber { get; set; }
    
    // notice how - for now - you must override Load_ with an underscore ("_")
    protected override void Load_(ContainerBuilder builder)
    {
        // register stuff, etc
    }
}
```

### set a module property
You can modify the properties of modules which are already loaded via the main XML file using the ```-s``` flag. Properties are comma-separated for now, I can change that once someone inevitably run into a case where we need commas in the value :-)

 Template:: ```-s:<module>:<property1>=<value1>,<property2>=<value2>```
 Example:: ```-s:MyModule:Foo=bar,baz=42```

Where ```<module>``` can either be the module type name (i.e. ```FooModule```) or an alias if compiled with one (i.e. ```foo```).

Note that no conflict resolution happens if the type name or alias of multiple modules is the same. Every module parses the command line independantly, so if they have the same alias or name setting a property once will apply to them all (if the property exists on them all, that is).

### load a new module
Using the ```-l``` flag you can load a module that was not already loaded in the XML. 

 Template:: ```-l:<ModuleType,ModuleAssembly>:<property>=<value>``` 
 Example (module load only):: ```-l:Name.Space.MyModule,MyAssembly```
 Example (with properties):: ```-l:Name.Space.MyModule,MyAssembly:Foo=bar,baz=42```

### load and set
Both flags can be used in combination:

Example:: ```-l:MyModule,MyAssembly -s:MyModule:Foo=bar -s:MyModule:Baz=42```

### Enabling and disabling a module
Even if a module is loaded in the XML, you can prevent it's loading using the ```on``` and ```off``` properties, which are by default implemented in the ```CommandLineAwareModule```.

 Example (full):: ```-s:MyModule:Enabled=false```
 Example (alias):: ```-s:MyModule:off```

### Aliases
Module names and property names can have shorter aliases for convenience. Command line settings (both module names and properties) will try the alias first (if they have one) but fall back to their type name if they either don't have an alias or no setting was found for their alias. This means usage of Alias is entirely optional, but convenient in cases where you use the command line a lot and don't want to type too much.

```csharp
[Alias("m")]
public class MyModule: CommandLineAwareModule
{
    [Alias("nr")]
    public int SomeNumber { get; set; }
}
```

Examples that will work on the command line with the above module:

```$>  MyApp.exe -s:MyModule:SomeNumber=42```

```$>  MyApp.exe -s:m:SomeNumber=42```

```$>  MyApp.exe -s:MyModule:nr=42```

```$>  MyApp.exe -s:m:nr=42```

## ScopeFactory<T>

A factory that when called will resolve T within it's own customized lifetime scope. The provided object will be scanned for it's properties, which will independently be registered as services in the new scope only. It's a convenient way to register or replace certain services within their own context and dynamically providing services to that context at runtime.

Why would anybody want that? In cases where you don't just want to resolve a particular instance (say via a FactoryDelegate or custom Factory class), but want it and all of it's dependencies to share custom services that are only known at runtime (=resolve time, as opposed to registration time).

### Example

I shall use the humble class "Foo" in the following examples.

```csharp
// our Foo expects an Id and ISomeThing to be injected via the constructor
class Foo {
      public Foo(int Id, ISomeThing bar) {
      	     // ...
      }
}
```

Registering a scoped factory is pretty straightforward.

```csharp
// register scoped factory for Foo manually
builder.RegisterType<ScopeFactory<Foo>>();

// or use the convenient helper extension
builder.RegisterScopeFactory<Foo>();
```

You can hook into the creation of a new lifetime scope and mofify the scope if you know what you want to do at registration time. This can be handy if you need to replace certain services whenever a Foo scope is created.

You have access to the parent scope, too, so you can resolve the original service if you need it. Here, SomethingElse will wrap the original ISomething which it fetches from the parent scope and will then make itself known as ISomething instead, but only within the scope of Foo and it's dependencies.

```csharp
// you can do it manually, but the extension method makes this much easier. it simply takes the event handler as an argument.
builder.RegisterScopeFactory<Foo>(parentScope, childBuilder) =>
   {
	// replace the ISomething service from the parent scope with SomethingElse in the new Foo scope
        
        childBuilder.RegisterType<SomethingElse>()
                    .WithParameter(parentScope.Resolve<ISomething>())
                    .As<ISomething>();

   });

```


```csharp
// resolve factory
var sf = c.Resolve<ScopeFactory<Foo>();
```

The Get() method takes an object on which the properties are scanned and registered as services of their specific type within the new scope. It's convenient to use anonoymous types here, but you can also use any object and it's properties will be scanned the same way.

Since Foo is still resolved through the container, other ctor dependencies will still be resolved through Autofac as usual. 

```csharp
// resolve a scoped Foo, where the service "Id" will be known in it's lifetime scope,
// available to Foo as well as any dependencies might need to be resolved in it's
// context. Any object can be provided for context, it's public properties will be scanned
// and registered
var scopedFoo = sf.Get(new{Id=123});
```

By casting the service's value/reference you can control which type the contextual service will be registered as. Say I want Id to be registered as long, not as it's implicit int...

```csharp
// force the Id service to be registered as service of type long
var scopedFoo = sf.Get(new{Id=(long)123});
```

The same applies to any reference type, where using "as" also reads nice and fluent.

```csharp
// force my instance at "bar" to be registered as service of type ICanHazCheezburger
// will obviously throw exceptions if bar is not assignable to ICanHazCheezburger
var scopedFoo = sf.Get(new{Id=123, Bar = bar as ICanHazCheezburger});
```

This all works best when Foo can resolve any additional ctor dependencies through the Autofac container. In cases where you must provide explicit ctor args, you can use the overload that takes an extra object array. The provided objects will be passed in as positional parameters, so ordering matters.

```csharp
// bar will become a service in the lifetime scoped of Foo but Id is now a ctor parameter for Foo only
// notice that since 123 is the first item in the args array, it must be the first ctor parameter on the
// Foo constructor
var scopedFoo = sf.Get(new{Bar=bar as ICanHazCheezburger}, new object[] {123});
```


## RegisterAndActivate<T>()

Registers a type and resolves it's dependencies like when doing RegisterType<>().
Once the container has been configured completely, a single instance of T
will be resolved (= activated). This is useful for components in your system
that are not themselves dependencies for other components, yet need to be
instantiated in order to the application to function correctly.

### Example

```csharp
// old-school
builder.RegisterAndActivate<Foo>();

// fancy-pants
builder.RegisterAndActivate<Foo>()
    .SingleInstance()
    .WithParamters(/* yadda yadda */);

```

## RegisterAndActivate<T>(Func<IComponentContext,T>)
Does the same thing, only that you can provide a delegate to resolve constructor
parameters, like when doing Register(c => ...)

### Example

```csharp
// old-school
builder.RegisterAndActivate<Foo>(c => new Foo(c.Resolve<ICanHazCheezburger>()));

// fancy-pants
builder.RegisterAndActivate<Foo>(c => new Foo(c.Resolve<ImInUrContainur>())
    .WithParameters(/* yadda yadda */)
    .OnActivated(/* yadda yadda */);
```

## Now included in Autofac 3
*NOTE: The RegisterAndActivate() concept as implemented here was adopted into the official Autofac libraries as of version 3.0.0 beta following my suggestion in ticket 388*
http://code.google.com/p/autofac/issues/detail?id=388
http://code.google.com/p/autofac/wiki/ReleaseNotes#3.0.0_Beta_2


```csharp
// Autofac 3
builder.RegisterType<MyClass>().AutoActivate();

```
The libraries you find here are compatible with earlier releases than Autofac 3 (haven't tried it, but at least as far back as 2.5).

build
============
Depends on just Autofac.
The solution uses NuGet, should be simples to download the deps and build.

See packages.config for up-to-date deps. Tested recently with Autofac 2.6.

license
=======
This code is licensed under the MIT license, which is included in the LICENSE file. You can also view it here: http://opensource.org/licenses/mit-license.php
Autofac is licensed and distributed separately. You should be able to get it here: http://code.google.com/p/autofac/
