using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class SmsVerify : ISmsVerify
    {
        
        private readonly string _baseUrl;
        private readonly string _authToken;
     
        
        private readonly IFourDigitCodeGenerator _codeGenerator; 
        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _dbContext; 
 

        public SmsVerify(IFourDigitCodeGenerator codeGenerator, ICurrentUserService currentUserService, ApplicationDbContext dbContext,  IOptions<SmsVerifyOptions> options
        )
        {
            _codeGenerator = codeGenerator;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _baseUrl = options.Value.BaseUrl;
            _authToken = options.Value.AuthToken;
        }
        
      
       
        public async Task SendSms()
        {
            // Получение id текущего юзера
            var currentUserId = _currentUserService.Id;

            // Поиск номера телефона по id юзера
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == currentUserId);

            if (user == null)
            {
                // Обработка ситуации, коли юзера не існує
                return;
            }

            var userPhoneNumber = user.Phone;

            
            string generatedCode = _codeGenerator.GenerateCode();
            string message = "Ваш код підтвердження для \nToaster 🍞 :\n📟 " + generatedCode + " \n \nНікому не передавайте цей код 🤫";
            string sender = "IT Alarm"; 
            string[] recipients = new string[] { userPhoneNumber };
            
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(_baseUrl);

                // Подготовка JSON
                var requestContent = new
                {
                    recipients,
                    sms = new
                    {
                        sender,
                        text = message
                    }
                };
                
                var jsonRequest = JsonConvert.SerializeObject(requestContent);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Добавление заголовков для аутентификации
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authToken);

                // Отправка запроса
                var response = await client.PostAsync("message/send.json", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("SMS відправлено. " + response);
                    // Сохранение кода в базу данных
                    user.PhoneVerifyCode = int.Parse(generatedCode);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("Помилка при надсиланні SMS: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Сталась помилка: " + ex.Message);
            }
        }
    }
}