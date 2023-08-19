using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckUpdates("PS3");
            CheckUpdates("PS4");
            CheckUpdates("PS5");
            CheckUpdates("PSVita");
        }

        static void CheckUpdates(string consoleName)
        {
            Console.WriteLine($"Checking updates for {consoleName}...");

            string updateUrl = GetUpdateUrl(consoleName);
            string updateInfo = FetchUpdateInfo(updateUrl);

            Console.WriteLine($"Latest updates for {consoleName}:\n{updateInfo}\n");

            string currentFirmwareVersion1 = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0001\", "CurrentProductVersionString", "");
            string currentFirmwareVersion2 = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0002\", "CurrentProductVersionString", "");
            string currentFirmwareVersion = currentFirmwareVersion1 + " - " + currentFirmwareVersion2;

            bool updateRequired = CurrentFirmwareVersionCheck(currentFirmwareVersion);

            if (updateRequired)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"An update is necessary for {consoleName}!");
                Console.ResetColor();

                Console.WriteLine("Do you want to update now? (1: Yes, 2: Later)");
                int userInput = Convert.ToInt32(Console.ReadLine());

                if (userInput == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Update will proceed now!");
                    Console.WriteLine("Please do not turn off the device during the update");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Ok, you will update later");
                }
            }
        }

        static bool CurrentFirmwareVersionCheck(string currentFirmwareVersion)
        {
            if (int.TryParse(currentFirmwareVersion, out int currentVersion))
            {
                int latestVersion = 123; 
                return currentVersion < latestVersion;
            }

            return false;
        }

        static string GetUpdateUrl(string consoleName)
            {
                switch (consoleName)
                {
                    case "PS3":
                        return "https://www.xtremeps3.com/firmware-history/";

                    case "PS4":
                        return "https://www.xtremeps3.com/ps4-firmware-history/";

                    case "PS5":
                        return "https://www.xtremeps3.com/ps5-firmware-history/";

                    case "PSVita":
                        return "https://www.xtremepsvita.com/firmware-history/";

                    default:
                        return "Invalid console name";
                }
            }

        static string FetchUpdateInfo(string url)
        {
            using (var client = new WebClient())
            {
                string contents = client.DownloadString(url);
                return contents;
            }
        }
    }
} 