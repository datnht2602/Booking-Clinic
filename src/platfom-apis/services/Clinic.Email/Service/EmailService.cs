using Clinic.Common.Options;
using Clinic.DTO.Models;
using Clinic.Email.Contract;
using Clinic.Email.Extension;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Clinic.Email.Service;

public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;
    private readonly IOptions<ApplicationSettings> applicationSettings;
    public EmailService(IEmailSender emailSender, IOptions<ApplicationSettings> applicationSettings)
    {
        _emailSender = emailSender;
        this.applicationSettings = applicationSettings;
    }
    public async Task SendInvoice(string email)
    {
        await _emailSender.SendEmailAsync(email, $"Invoice Booking",
            "Test");
    }

    public async Task ChangeSchedule(BookingDetailsViewModel model)
    {
        await _emailSender.SendEmailAsync(model.BriefViewModel.Email, $"Change Schedule",
            BodyContent(model));
    }
    private string BodyContent(BookingDetailsViewModel model)
    {
        return
        $@" <div>
        <table style=""border-collapse:collapse;table-layout:fixed;border-spacing:0;vertical-align:top;min-width:320px;background-color:#ffffff;width:100%;margin-top:-100px;margin-bottom:-85px"" cellpadding=""0"" cellspacing=""0"">
            <tbody>
                <tr style=""vertical-align:top"">
                    <td style=""word-break:break-word;border-collapse:collapse!important;vertical-align:top"">
                        <div style=""background-color:transparent"">
                            <div style=""min-width:320px;max-width:450px;word-wrap:break-word;word-break:break-word;background-color:transparent"" class=""m_3727995092631990582block-grid"">
                                <div style=""border-collapse:collapse;display:table;width:100%;background-color:transparent"">
                                    <div class=""m_3727995092631990582col"" style=""min-width:320px;max-width:500px;display:table-cell;vertical-align:top"">
                                        <div style=""background-color:transparent;width:100%!important"">
                                            <div style=""border-top:0px solid transparent;border-left:0px solid transparent;border-bottom:0px solid transparent;border-right:0px solid transparent;padding-top:5px;padding-bottom:5px;padding-right:0px;padding-left:0px"">
                                                <div style=""font-size:16px;font-family:Arial,'Helvetica Neue',Helvetica,sans-serif;text-align:center"">
                                                    <div style=""display: flex; justify-content: center; align-items: center; height: 100vh;"">
                                                        <div style=""display: flex; justify-content: center;  gap: 20px;"">
                                                            <table align=""center"" style=""margin:0 auto;width:450px;height:600px;border:5px solid #e0e6e8;padding:5px"">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style=""text-align:center""><b>4Health Clinic</b></td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td style=""text-align:center"">
                                                                            <b>Clinic Number: {model.ClinicNum}</b>
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
                                                                                <b>14:30</b>
                                                                            </p>
                                                                            <p style=""padding:0;margin:8px 0 0"">
                                                                                (Estimated examination time)
                                                                            </p>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style=""text-align:left;padding:15px"">Date: <b>{model.OrderPlacedDate.GetDateTime().ToString("yyyy-MM-dd")}</b></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style=""text-align:left;padding:15px"">Service: <b>{string.Join(",",model.Products.Select(x => x.Name))}</b></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style=""text-align:left;padding:15px"">Doctor: <b>{model.DoctorName}</b></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style=""text-align:left;padding:15px"">Price: <b>{model.OrderTotal.ToString("C")} VND</b></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <div style=""display: flex; justify-content: center; margin-left: 15px;"">
                                                            <table align=""center"" style=""margin: 0 auto; width: 450px; height: 600px; border: 5px solid #e0e6e8; padding: 5px; display: flex; flex-direction: column; justify-content: space-between;"">
                                                                <tr>
                                                                    <td colspan=""2"" style=""text-align: center; vertical-align: bottom; padding-top: 60px; padding-bottom: 10px"">
                                                                        <p>
                                                                            <h4 style=""font-weight: bold; padding-bottom: 20px;"">
                                                                                <br />This is a notice to reschedule your appointment.<br />
                                                                            </h4>
                                                                            For some reason, your appointment cannot go as planned.<br />
                                                                            Would you like to continue your appointment at this time but with a different doctor?<br />
                                                                            If not, please choose another time to continue.
                                                                        </p>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style=""text-align: center; vertical-align: middle; padding: 10px"">
                                                                        <button style=""width: 250px; height: 50px; background-color: white; border-color: #48a7f2; border-radius: 8px; padding: 10px 24px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;"">
                                                                            <a href=""{this.applicationSettings.Value.ClientApiEndpoint}/changedoctor/{model.Id}"">Choose another doctor</a></button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style=""text-align: center; vertical-align: middle; padding: 10px"">
                                                                        <button style=""width: 250px; height: 50px; background-color: white; border-color: #48a7f2; border-radius: 8px; padding: 10px 24px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;"">
                                                                            <a href=""{this.applicationSettings.Value.ClientApiEndpoint}/changeschedule/{model.Id}"">Choose another time</a></button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

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