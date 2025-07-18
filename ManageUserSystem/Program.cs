using System.Text;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using ManageUserSystem.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Đăng ký IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddScoped<DynamicPermissionFilter>();

builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
