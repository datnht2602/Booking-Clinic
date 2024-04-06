using Clinic.DTO.Models.Dto;
using Clinic.Email.Contract;
using Clinic.Email.Extension;
using Clinic.Email.MailKitService;
using Clinic.Email.Service;
using Mango.Services.Email.Messaging;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapPost("/resetpassword",async (SendMailDto dto,IEmailSender _emailSender) =>{
        await _emailSender.SendEmailAsync(dto.Email, $"Reset Password",
            $"To Reset Password click the link <a href='{dto.CallBackUrl}'>link here</a>." );
        return Results.Ok();
    })
    .WithOpenApi();
app.MapPost("/testmail",async (string email,IEmailService emailService) =>
    {
        emailService.SendInvoice(email);
        return Results.Ok();
    })
    .WithOpenApi();
app.UseAzureServiceBusConsumer();
app.Run();


