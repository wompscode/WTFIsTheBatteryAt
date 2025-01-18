using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using static WTFIsTheBatteryAt.Logging;

namespace WTFIsTheBatteryAt
{
    internal class UpdateChecker
    {
        public class UpdateStatus
        {
            public VersionCheck state = VersionCheck.other;
            public string current = "";
            public string requested = "";
        }
        public enum VersionCheck
        {
            latest,
            out_of_date,
            non_success,
            other
        }
        private static readonly HttpClient client = new HttpClient();

        private readonly static string updateURL = "https://raw.githubusercontent.com/wompscode/WTFIsTheBatteryAt/refs/heads/master/version";
        public static async Task<UpdateStatus> CheckForUpdateAsync(string version)
        {
            Log($"requesting version from {updateURL}", "[UpdateChecker]");

            HttpResponseMessage response = await client.GetAsync(updateURL);

            UpdateStatus state = new UpdateStatus();
            state.current = version;

            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                string bodyTrimmed = body.Trim();
                Log($"successful response: {bodyTrimmed}", "[UpdateChecker]");
                Log($"current: {version}, requested: {bodyTrimmed}", "[UpdateChecker]");
                if (version == bodyTrimmed)
                {
                    state.requested = bodyTrimmed;
                    state.state = VersionCheck.latest;
                } else
                {
                    state.requested = bodyTrimmed;
                    state.state = VersionCheck.out_of_date;
                }
            } else
            {
                Log($"unsuccessful response: {response.StatusCode}", "[UpdateChecker]");
                state.state = VersionCheck.non_success;
            }
            return state;
        }
    }
}
