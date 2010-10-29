﻿namespace Caliburn.ShellFramework.Resources
{
    using System;
    using System.IO;
    using System.Windows;
    using Core.Logging;

    public class DefaultResourceManager : IResourceManager
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(DefaultResourceManager));

        public Stream GetStream(string relativeUri, string assemblyName)
        {
            try
            {
                var resource = Application.GetResourceStream(new Uri(assemblyName + ";component/" + relativeUri, UriKind.Relative))
                               ?? Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));

                if (resource != null)
                    return resource.Stream;

                Log.Warn("Resource {0} not found in {1}.", relativeUri, assemblyName);
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
    }
}