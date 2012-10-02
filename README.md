autofac-extensions
==================
Some nice helpers for when working with the great .NET IoC container Autofac.

## RegisterAndActivate<T>()
Registers a type and resolves it's dependencies like when doing RegisterType<>().
Once the container has been configured completely, a single instance of T
will be resolved (= activated). This is useful for components in your system
that are not themselves dependencies for other components, yet need to be
instantiated in order to the application to function correctly.

### Example

```csharp
// old-school
builder.RegisterAndActivate<MyClass>();

// fancy-pants
builder.RegisterAndActivate<MyClass>()
    .SingleInstance()
    .WithParamters(/* yadda yadda */);

```

## RegisterAndActivate<T>(Func<IComponentContext,T>)
Does the same thing, only that you can provide a delegate to resolve constructor
parameters, like when doing Register(c => ...)

### Example

```csharp
// old-school
builder.RegisterAndActivate<MyClass>(c => new MyClass(c.Resolve<ICanHazCheezburger>()));

// fancy-pants
builder.RegisterAndActivate<MyClass>(c => new MyClass(c.Resolve<ImInUrContainur>())
    .WithParameters(/* yadda yadda */)
    .OnActivated(/* yadda yadda */);
```


build
============
Depends on just Autofac.
The solution uses NuGet, should be simples to download the deps and build.

See packages.config for up-to-date deps. Tested recently with Autofac 2.6.

license
=======
This code is licensed under the MIT license, which is included in the LICENSE file. You can also view it here: http://opensource.org/licenses/mit-license.php
Autofac is licensed and distributed separately. You should be able to get it here: http://code.google.com/p/autofac/