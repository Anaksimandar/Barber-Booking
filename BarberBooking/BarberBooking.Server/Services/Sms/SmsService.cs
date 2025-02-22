using BarberBooking.Server.Models.File;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.Json;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BarberBooking.Server.Services.Sms
{
    public class SmsService : ISmsService
    {
        private readonly string accountId;
        private readonly string token;
        private readonly string senderNumber;
        public SmsService()
        {
            string twilioConfigFile = "C:\\Users\\acode\\Source\\Repos\\Barber-Booking\\BarberBooking\\BarberBooking.Server\\Files\\twilio-config.json";
            string twilioConfigJson = File.ReadAllText(twilioConfigFile);
            TwilioConfig? twilioConfigModel = JsonSerializer.Deserialize<TwilioConfig>(twilioConfigJson);
            this.accountId = twilioConfigModel.AccountId;
            this.token = twilioConfigModel.AuthToken;
            this.senderNumber = twilioConfigModel.TwilioNumber;
            TwilioClient.Init(accountId, token);
        }

        public async Task SendConfirmation(string to, string body)
        {
            string pattern = @"^(?:\+3816|06)[1-6]\d{6,7}$";
            if (to == null || to.Length == 0) {
                throw new ArgumentException("Please provide number");
            }
            if (!Regex.IsMatch(to, pattern)) {
                throw new Exception("Bad format of number");
            }

            to = "+381" + to.Substring(1);

            var message = await MessageResource.CreateAsync(
                body: body,
                from: senderNumber,
                to: to
            );
        }

        public async Task SendReminder(string to)
        {
            to = "+381" + to.Substring(1);

            string body = @$"Your service will start in 2 hours.
                If you want to change date please follow next link http://localhost:4200/list-reservations";

            var message = await MessageResource.CreateAsync(
                body: body,
                from: senderNumber,
                to: to
            );

        }
    }
}
