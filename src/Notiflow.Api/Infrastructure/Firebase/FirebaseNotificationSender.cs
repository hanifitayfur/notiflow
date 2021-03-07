using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Notiflow.Api.Infrastructure.Firebase
{
    public interface IFirebaseNotificationSender
    {
        Task<FirebaseNotificationResponseModel> SendAsync(string serverKey, string senderId, List<string> deviceIds,
            object payload);
    }

    public class FirebaseNotificationSender : IFirebaseNotificationSender
    {
        private readonly FirebaseAppSettings _firebaseAppSettings;
        private readonly string _firebaseUrl = "https://fcm.googleapis.com/fcm/send";

        public FirebaseNotificationSender(IOptionsSnapshot<FirebaseAppSettings> firebaseAppSettings)
        {
            if (firebaseAppSettings != null)
            {
                _firebaseAppSettings = firebaseAppSettings.Value;
                _firebaseUrl = _firebaseAppSettings.FirebaseUrl;
            }
        }

        public Task<FirebaseNotificationResponseModel> SendAsync(string serverKey, string senderId,
            List<string> deviceIds,
            object payload)
        {
            var jsonObject = JObject.FromObject(payload);
            jsonObject.Remove("registration_ids");
            jsonObject.Add("registration_ids", JToken.FromObject(deviceIds));

            return SendAsync(serverKey, senderId, jsonObject);
        }

        private async Task<FirebaseNotificationResponseModel> SendAsync(string serverKey, string senderId,
            object payload)
        {
            string serialized = JsonConvert.SerializeObject(payload);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, _firebaseUrl);
            httpRequest.Headers.Add("Authorization", $"key = {serverKey}");
            httpRequest.Headers.Add("Sender", $"id = {senderId}");

            httpRequest.Content = new StringContent(serialized, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Firebase notification error: " + responseString);
            }

            return JsonConvert.DeserializeObject<FirebaseNotificationResponseModel>(responseString);
        }
    }
}