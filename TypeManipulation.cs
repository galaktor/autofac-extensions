// Copyright (c)  2013 Raphael Estrada
// License:       The MIT License - see "LICENSE" file for details
// Author URL:    http://www.galaktor.net
// Author E-Mail: galaktor@gmx.de

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace Autofac.Configuration.Util
{
    // copied and slightly modified since the original one in the Aufoac sources is internal
    public static class TypeManipulation
    {
        // - turned into an extension method for convenience
        // - renamed from ChangeToCompatibleType
        public static object ConvertTo(this object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if (value == null)
                return destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;

            //is there an explicit conversion
            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter.CanConvertTo(destinationType))
                return converter.ConvertTo(value, destinationType);

            //is there an implicit conversion
            if (destinationType.IsInstanceOfType(value))
                return value;

            //is there an opposite conversion
            converter = TypeDescriptor.GetConverter(destinationType);
            if (converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(value);

            //is there a TryParse method
            if (value is string)
            {
                MethodInfo parser = destinationType.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public);
                if (parser != null)
                {
                    var parameters = new[] {value, null};
                    if ((bool) parser.Invoke(null, parameters))
                        return parameters[1];
                }
            }

            throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "Unable to convert object of type '{0}' to type '{1}'.", value.GetType(), destinationType));
        }
    }
}