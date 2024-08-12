using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using SchoolApi.Models;
using SchoolApi.Models.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
builder.Services.AddControllers(opt =>
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//.AddNewtonsoftJson();
builder.Services.AddScoped<IValidator<Student>, StudentValidator>();
builder.Services.AddScoped<IValidator<Course>, CourseValidator>();
builder.Services.AddScoped<IValidator<Application>, ApplicationValidator>();
builder.Services.AddDbContext<SchoolDbContext>(opt =>
    opt.UseInMemoryDatabase("SchoolDb"));
builder.Services.AddCors();

// builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
        SchoolDbContextSeed.SeedAsync(context);
    }
}

// app.

app.UseCors(
        options => options.WithOrigins("http://localhost:9000").AllowAnyMethod().AllowAnyHeader()
    );



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
