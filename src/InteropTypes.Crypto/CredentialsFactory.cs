using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NuGet.Configuration;



namespace InteropTypes.Crypto
{
    
    public static class CredentialsFactory
    {
        /// <summary>
        /// Retrieves the local credentials for a given package source name.
        /// </summary>
        public static System.Net.NetworkCredential CreateFromNugetConfig(string packageSourceName, System.IO.DirectoryInfo context = null)
        {
            // try with Nuget.Config:            
            var credentials = NuGet.Configuration.Settings
                .LoadDefaultSettings(context.FullName)
                ?.GetSection("packageSourceCredentials")
                ?.Items
                ?.OfType<NuGet.Configuration.CredentialsItem>()
                ?.FirstOrDefault(item => item.ElementName == packageSourceName);

            if (credentials == null) return null;

            /*
             * One tool that you can use to encrypt clear text passwords of Nuget.Config is conf-encrypt,
             * which is a .NET global tool that allows you to encrypt and decrypt any configuration value using a public key or a certificate.
             * You can install it globally using the following command:
             * dotnet tool install -g conf-encrypt
             * And then run it using the following command:
             * conf-encrypt encrypt "/path/to/the/public-key-or-certificate" "Content To Encrypt"
             * NOTE: conf-encrypt has not been updated and requires NET5 runtime
             */

            // handles decrypting non clear text passwords
            var packageCredentials = new NuGet.Configuration.PackageSourceCredential
                (
                packageSourceName,
                credentials.Username,
                credentials.Password,
                credentials.IsPasswordClearText,
                credentials.ValidAuthenticationTypes
                );

            return new System.Net.NetworkCredential(packageCredentials.Username, packageCredentials.Password);
        }


        public static bool EncryptNugetConfigClearTextPasswords(System.IO.FileInfo finfo)
        {            
            var settings = NuGet.Configuration.Settings.LoadSpecificSettings(finfo.Directory.FullName, finfo.Name);
            return EncryptNugetConfigClearTextPasswords(settings);
        }

        public static bool EncryptNugetConfigClearTextPasswords(System.IO.DirectoryInfo dinfo)
        {
            var settings = NuGet.Configuration.Settings.LoadSpecificSettings(dinfo.FullName, "nuget.config");
            return EncryptNugetConfigClearTextPasswords(settings);
        }

        public static bool EncryptNugetConfigClearTextPasswords(ISettings settings)
        {
            // Get the packageSources section
            var packageSourceCreds = settings.GetSection("packageSourceCredentials");

            var items = packageSourceCreds
                .Items
                .OfType<NuGet.Configuration.CredentialsItem>()
                .Where(item => item.IsPasswordClearText)
                .ToList();

            if (items.Count == 0) return false;

            foreach (var item in items)
            {
                var xpass = EncryptionUtility.EncryptString(item.Password);
                item.UpdatePassword(xpass, false);
                settings.AddOrUpdate("packageSourceCredentials", item);
            }

            // Save the settings to the same file
            settings.SaveToDisk();

            return true;
        }
    }
}
