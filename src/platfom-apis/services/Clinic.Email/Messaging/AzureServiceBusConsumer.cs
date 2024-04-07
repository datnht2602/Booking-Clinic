using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Message;
using Clinic.Email.Extension;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Mango.Services.Email.Messaging
{  
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmail;
        private readonly string checkoutMessageTopic;

        private readonly IEmailSender _emailSender;

        private ServiceBusProcessor checkOutProcessor;
        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer( IConfiguration configuration,IEmailSender emailSender)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            checkoutMessageTopic = _configuration.GetValue<string>("checkoutmessagetopic");
            _emailSender = emailSender;

            var client = new ServiceBusClient(serviceBusConnectionString);

            orderUpdatePaymentStatusProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionEmail);
        }

        public async Task Start()
        {
          
            orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            
            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

  

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            BookingDetailDto checkoutHeaderDto = JsonConvert.DeserializeObject<BookingDetailDto>(body);
            try
            {
                await _emailSender.SendEmailAsync(checkoutHeaderDto.BriefViewModel.Email, $"Notification booking of {checkoutHeaderDto.DoctorName}",
                   ReturnBodyContent(checkoutHeaderDto) );
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string ReturnBodyContent(BookingDetailDto information)
        {
            return
                @$"<div>
    <span class=""im"" align=""center"" style=""padding-10px"">
        <h3>
            Thank you for using our online medical registration service, here is your medical examination form <br/>
            Please bring this medical examination card with you when you come for examination at the hospital. Thank you very much!
        </h3>
    </span><table style=""border-collapse:collapse;table-layout:fixed;border-spacing:0;vertical-align:top;min-width:320px;Margin:0 auto;background-color:#ffffff;width:100%"" cellpadding=""0"" cellspacing=""0"">
        <tbody>
            <tr style=""vertical-align:top"">
                <td style=""word-break:break-word;border-collapse:collapse!important;vertical-align:top"">

                    <div style=""background-color:transparent"">
                        <div style=""Margin:0 auto;min-width:320px;max-width:500px;word-wrap:break-word;word-break:break-word;background-color:transparent"" class=""m_3727995092631990582block-grid"">
                            <div style=""border-collapse:collapse;display:table;width:100%;background-color:transparent"">


                                <div class=""m_3727995092631990582col"" style=""min-width:320px;max-width:550px;display:table-cell;vertical-align:top"">
                                    <div style=""background-color:transparent;width:100%!important"">

                                        <div style=""border-top:0px solid transparent;border-left:0px solid transparent;border-bottom:0px solid transparent;border-right:0px solid transparent;padding-top:5px;padding-bottom:5px;padding-right:0px;padding-left:0px"">

                                            <div align=""center"" class=""m_3727995092631990582fullwidthOnMobile"" style=""padding-right:0px;padding-left:0px"">
                                            </div>

                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div style=""background-color:transparent"">
                        <div style=""Margin:0 auto;min-width:320px;max-width:500px;word-wrap:break-word;word-break:break-word;background-color:transparent"" class=""m_3727995092631990582block-grid"">
                            <div style=""border-collapse:collapse;display:table;width:100%;background-color:transparent"">


                                <div class=""m_3727995092631990582col"" style=""min-width:320px;max-width:500px;display:table-cell;vertical-align:top"">
                                    <div style=""background-color:transparent;width:100%!important"">

                                        <div style=""border-top:0px solid transparent;border-left:0px solid transparent;border-bottom:0px solid transparent;border-right:0px solid transparent;padding-top:5px;padding-bottom:5px;padding-right:0px;padding-left:0px"">

                                            <div style=""font-size:16px;font-family:Arial,'Helvetica Neue',Helvetica,sans-serif;text-align:center"">
                                                <div>

                                                    <table align=""center"" style=""margin:0 auto;width:450px;height:746px;border:5px solid #e0e6e8;padding:5px"">
                                                        <tbody>
                                                            <tr>
                                                                <td style=""text-align:center""><b>4Health Clinic</b></td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td style=""text-align:center"">
                                                                    <b>Appointment Code</b>
                                                                    <br>
                                                                    <img align=""center"" border=""0"" src=""https://ci3.googleusercontent.com/meips/ADKq_Nap5tWjtOHtpuAaKnQG29WCdTFFfALkLGzve3vSpKpTXqxdheFEeIHpQb6EgKN78MiB8PfAoHKujURSyoxJ_oVgdNpV9f5kY8QxVv7-H1q8CBiHuCSF7wKYc9_ai--qAmi7Pz6qkiYCuyr9hwXFwCyhFjnV61JQNf7qXQI3k_CzLxw=s0-d-e1-ft#https://cdn.medpro.vn/prod-benhvien199/7cfa1279-c22a-43f2-b678-ef3ca29d9046-37252bf7bc4e45018c3aab70bd24d43e"" alt=""barcode"" title=""barcode"" class=""CToWUd"" data-bit=""iit"">
                                                                    <br>
                                                                    T240304NZC557
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style=""text-align:center"">
                                                                </td>
                                                            </tr>

                                                            <tr style=""display:flex"">

                                                                <td style=""text-align:center;height:110px;width:100%"">
                                                                    <p style=""color:#48a7f2;font-size:48px;margin:0;font-weight:bold"">
                                                                        <b>{information.OrderPlacedDate.GetDateTime().ToString("hh:mm tt")}</b>
                                                                    </p>
                                                                    <p style=""padding:0;margin:8px 0 0"">
                                                                        (Estimated examination time)
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""display:inline-block;margin-top:5px;padding:5px 20px;border-radius:20px;background-color:#f26f21;color:#ffffff"">Booking Successful</td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:center;padding:0px;margin:0px""><p style=""margin:0px;color:#ff4500"">Please Pay Attention</p></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:center;padding:0px;margin:0px""><p style=""margin:0px;margin-bottom:10px;color:#ff4500""></p></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Clinic Number: <b>{information.BriefViewModel.ClinicNumber}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Date: <b>{information.OrderPlacedDate.GetDateTime().ToString("yyyy-MM-dd")}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Service: <b>{string.Join(",",information.Products.Select(x => x.Name))}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Doctor: <b>{information.DoctorName}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Name: <b>{information.UserName}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Gender: <b>{(information.BriefViewModel.Sexual == 1 ? "Male" : "Female")}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Date of birth: <b>{information.BriefViewModel.DateOfBirth}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Healh Insurance Code: <b>{information.BriefViewModel.HealthInsuranceCode}</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left;padding:10px"">Price: <b>{information.OrderTotal.ToString("C")} VND</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <hr style=""color:#e0e6e8;margin-bottom:1px;margin-top:1px"">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style=""text-align:left"">
                                                                    <i>
                                                                        <div>
                                                                            <ul start=""1"">
                                                                                <li>
                                                                                    <p>Please arrive 15 minutes before the appointment time to receive confirmation of information and instructions for entering the examination.</p>
                                                                                </li>
                                                                            </ul>
                                                                            <div style=""padding: 10px"">Note:</div>
                                                                            <ul start=""1"">
                                                                                <li>
                                                                                    <p>Patients are guaranteed to arrive for examination at the scheduled appointment time.</p>
                                                                                </li>
                                                                                <li>
                                                                                    <p>Outside of the above time frame, the examination form is still valid on the day of the examination <strong>7:00</strong> to <strong>17:00</strong>, reception will be arranged according to hospital regulations.</p>
                                                                                </li>
                                                                                <li>
                                                                                    <p>If any problems arise that require support, please contact Hotline <strong>098 xxx xxxx</strong> for quick handling.</p>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                    </i>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>
        </tbody>
    </table>

    <img src=""https://ci3.googleusercontent.com/meips/ADKq_NaKGd5hN3Yx7TVrMNxYl8z6Y3FXdumN2eBkn0bnDYz3wxsaoD3i3Bz7u8y4_5W9LdoHnKIqn2qDu-GsPoLXxU4WTqpKo8NPuisKbz-TVeMQSj_q1v-h_HWK1VXXoMZuxMY4LMtbe7GJvMHLUNdn781-Vb4HrcDtCAFpt2W2moTkJUGl9FWCbGE1sbxVX6UrnEzbT1lOGQRrVsgXuafO_HOwzvlGe13EUe8xsoG5hbEGS19OYq6w_pMCDLGSxwbXSLZkGlg1D2SKjbAeXnSA1o7C9REodoej_pv3zwglS6A3tSw3dhWIGdWmH3B0b37e8gCbXpC4V8YOfTeb97Dol22lM8eosKPNt7yUwblV5wlExsn-Joncuv9scy72QJ6dYj7l2lKTyhFpMmLjA0gzCMGuZYSO4sHv2ezwLg=s0-d-e1-ft#https://u7537279.ct.sendgrid.net/wf/open?upn=u001.7kMSbMYBhoMhZJKufo3REQspxxGeO06LPaSkFydzpZoxmfgZW3V7Zvz-2FTHqtclcX69vDdCVBbI21PI5GaQU6byBXJeLQxtxY7c5M8n6mtrF6tWJTyKEb1EOyaesoOlt0EvgEqUtbjinSR0cghl4d3MDugASWxToIhusTMf27rNcgnyqD4iyPTz8wFg7Myik1FEZ79FfsQ61-2BBwv2B0WNhjDShafQr3oEsfIBQzgjvNk-3D"" alt="""" width=""1"" height=""1"" border=""0"" style=""height:1px!important;width:1px!important;border-width:0!important;margin-top:0!important;margin-bottom:0!important;margin-right:0!important;margin-left:0!important;padding-top:0!important;padding-bottom:0!important;padding-right:0!important;padding-left:0!important"" class=""CToWUd"" data-bit=""iit"">
</div>";
        }
    }
}
