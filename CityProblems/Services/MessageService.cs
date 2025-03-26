using System.Net.Mail;
using System.Net;
using CityProblems.DataAccess.Entities;

namespace CityProblems.Services
{
    public class MessageService : IMessageService
    {
        public bool Send(UserEntity receiver, UserEntity sender, IssueEntity issue, bool isForWorker=false)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.mail.ru")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("maqd@list.ru", "quPTnRJrfmW1b2PprUmW"),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("maqd@list.ru"),
                    Subject = isForWorker ? "Новая задача" : "Статус решения проблемы",
                    IsBodyHtml = true
                };

                string finalEmail;

                if (!isForWorker)
                {
                    string emailTemplate = @"
<!DOCTYPE html>
<html lang=""ru"">
<body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; max-width: 800px; margin: 0 auto; padding: 20px; background-color: #f9f9f9;"">
    <div style=""background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08); overflow: hidden;"">
        <div style=""background: linear-gradient(135deg, #4e73df 0%, #224abe 100%); color: white; padding: 30px; text-align: center;"">
            <h1 style=""margin: 0; font-size: 24px;"">Статус решения проблемы</h1>
            <p style=""margin: 10px 0 0; font-size: 16px;"">Информация об обращении в службу поддержки</p>
        </div>
        
        <div style=""padding: 30px;"">
            <div style=""margin-bottom: 25px; padding-bottom: 25px; border-bottom: 1px solid #eee;"">
                <h2 style=""color: #4e73df; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Информация об отправителе</h2>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Имя пользователя:</div>
                    <div style=""display: table-cell;"">{SenderName}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Email:</div>
                    <div style=""display: table-cell;"">{SenderEmail}</div>
                </div>
            </div>
            
            <div style=""margin-bottom: 25px; padding-bottom: 25px; border-bottom: 1px solid #eee;"">
                <h2 style=""color: #4e73df; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Информация о проблеме</h2>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">ID заявки:</div>
                    <div style=""display: table-cell;"">{IssueId}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Дата:</div>
                    <div style=""display: table-cell;"">{IssueDate}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Категория:</div>
                    <div style=""display: table-cell;"">{IssueCategory}</div>
                </div>
                <div style=""margin-top: 15px;"">
                    <div style=""font-weight: 600; color: #6c757d; margin-bottom: 5px;"">Описание:</div>
                    <p style=""margin: 0;"">{IssueDescription}</p>
                </div>
            </div>
            
            <div style=""margin-bottom: 25px;"">
                <h2 style=""color: #4e73df; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Прогресс решения</h2>
                <div style=""height: 10px; background-color: #f0f0f0; border-radius: 5px; margin: 15px 0; overflow: hidden;"">
                    <div style=""height: 100%; border-radius: 5px; background: linear-gradient(90deg, #4e73df 0%, #224abe 100%); width: {ProgressPercentage}%;""></div>
                </div>
                <div style=""text-align: center; margin-top: 10px;"">
                    <span style=""display: inline-block; padding: 5px 15px; border-radius: 20px; font-weight: 600; font-size: 14px; background-color: {StatusColor}; color: #fff;"">{StatusText}</span>
                </div>
            </div>
        </div>
        
        <div style=""text-align: center; padding: 20px; color: #6c757d; font-size: 14px; border-top: 1px solid #eee;"">
            <p style=""margin: 0 0 5px;"">© {CurrentYear} Город живой. Все права защищены.</p>
            <p style=""margin: 0;"">Это письмо сформировано автоматически, пожалуйста, не отвечайте на него.</p>
        </div>
    </div>
</body>
</html>";

                    string statusColor = issue.ExecutionState == ExecutionState.InProgress ? "#f6c23e" : "#1cc88a";
                    string statusText = issue.ExecutionState == ExecutionState.InProgress ? "В работе" : "Решено";
                    int progressPercentage = issue.ExecutionState == ExecutionState.InProgress ? 50 : 100;

                    finalEmail = emailTemplate
                        .Replace("{SenderName}", sender.Username)
                        .Replace("{SenderEmail}", sender.Email)
                        .Replace("{IssueId}", issue.Id.ToString())
                        .Replace("{IssueDate}", issue.CreatedAt.ToString("dd.MM.yyyy HH:mm"))
                        .Replace("{IssueCategory}", issue.Category.Title)
                        .Replace("{IssueDescription}", issue.Description)
                        .Replace("{ProgressPercentage}", progressPercentage.ToString())
                        .Replace("{StatusColor}", statusColor)
                        .Replace("{StatusText}", statusText)
                        .Replace("{CurrentYear}", DateTime.Now.Year.ToString());
                }
                else
                {
                    string workerTemplate = @"
<!DOCTYPE html>
<html lang=""ru"">
<body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; max-width: 800px; margin: 0 auto; padding: 20px; background-color: #f9f9f9;"">
    <div style=""background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08); overflow: hidden;"">
        <div style=""background: linear-gradient(135deg, #ff6b6b 0%, #ff8e8e 100%); color: white; padding: 30px; text-align: center;"">
            <h1 style=""margin: 0; font-size: 24px;"">Новая задача</h1>
            <p style=""margin: 10px 0 0; font-size: 16px;"">Требуется ваше внимание</p>
        </div>
        
        <div style=""padding: 30px;"">
            <div style=""margin-bottom: 25px; padding-bottom: 25px; border-bottom: 1px solid #eee;"">
                <h2 style=""color: #ff6b6b; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Детали задачи</h2>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">ID заявки:</div>
                    <div style=""display: table-cell;"">{IssueId}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Дата:</div>
                    <div style=""display: table-cell;"">{IssueDate}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Категория:</div>
                    <div style=""display: table-cell;"">{IssueCategory}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Приоритет:</div>
                    <div style=""display: table-cell;"">{Priority}</div>
                </div>
            </div>
            
            <div style=""margin-bottom: 25px; padding-bottom: 25px; border-bottom: 1px solid #eee;"">
                <h2 style=""color: #ff6b6b; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Описание проблемы</h2>
                <p style=""margin: 0; padding: 10px; background-color: #f8f9fa; border-radius: 5px;"">{IssueDescription}</p>
            </div>
            
            <div style=""margin-bottom: 25px;"">
                <h2 style=""color: #ff6b6b; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Местоположение</h2>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Широта:</div>
                    <div style=""display: table-cell;"">{Latitude}</div>
                </div>
                <div style=""display: table; width: 100%; margin-bottom: 8px;"">
                    <div style=""display: table-cell; width: 120px; font-weight: 600; color: #6c757d;"">Долгота:</div>
                    <div style=""display: table-cell;"">{Longitude}</div>
                </div>
                <div style=""margin-top: 15px; text-align: center;"">
                    <a href=""https://yandex.ru/maps/?pt={Longitude},{Latitude}&z=18&l=map"" style=""display: inline-block; padding: 10px 20px; background-color: #ff6b6b; color: white; text-decoration: none; border-radius: 5px; font-weight: 600;"">Открыть в Яндекс.Картах</a>
                </div>
            </div>
            
            <div style=""background-color: #f8f9fa; padding: 15px; border-radius: 5px;"">
                <h2 style=""color: #ff6b6b; font-size: 18px; font-weight: 600; margin-bottom: 15px;"">Действия</h2>
                <p style=""margin: 0;"">Пожалуйста, примите задачу в работу в течение 24 часов.</p>
            </div>
        </div>
        
        <div style=""text-align: center; padding: 20px; color: #6c757d; font-size: 14px; border-top: 1px solid #eee;"">
            <p style=""margin: 0 0 5px;"">© {CurrentYear} Город живой. Все права защищены.</p>
            <p style=""margin: 0;"">Это письмо сформировано автоматически.</p>
        </div>
    </div>
</body>
</html>";

                    string priority = "Средний"; 
                    if (issue.ExecutionState == ExecutionState.InProgress) priority = "Высокий";
                    if (issue.CreatedAt < DateTime.Now.AddDays(-3)) priority = "Критический";

                    finalEmail = workerTemplate
                        .Replace("{IssueId}", issue.Id.ToString())
                        .Replace("{IssueDate}", issue.CreatedAt.ToString("dd.MM.yyyy HH:mm"))
                        .Replace("{IssueCategory}", issue.Category.Title)
                        .Replace("{IssueDescription}", issue.Description)
                        .Replace("{Latitude}", issue.Latitude.ToString())
                        .Replace("{Longitude}", issue.Longitude.ToString())
                        .Replace("{Priority}", priority)
                        .Replace("{CurrentYear}", DateTime.Now.Year.ToString());
                }

                mailMessage.Body = finalEmail;
                mailMessage.To.Add(receiver.Email);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}