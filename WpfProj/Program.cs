//using CoreWCF.Configuration;
//using CoreWCF.Description;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;

//var builder = WebApplication.CreateBuilder(args);

//// Add CoreWCF services
//builder.Services.AddServiceModelServices();
//builder.Services.AddServiceModelMetadata();
//builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

//var app = builder.Build();

//app.UseServiceModel(app, builder =>
//{
//    // TODO add here service endpoints and behaviors
//});

//var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
//serviceMetadataBehavior.HttpGetEnabled = true;

//app.Run();
