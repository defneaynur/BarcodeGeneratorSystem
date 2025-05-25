using BarcodeGeneratorSystem.Api.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();          

builder.BaseAppSettings();
builder.BaseInject();
builder.BaseConfigure();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/BarcodeGeneratorSystem/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; 
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine("Received Token: " + token);
    await next.Invoke();
});

app.UseCors("myAppCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


