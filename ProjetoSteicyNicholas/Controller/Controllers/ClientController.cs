using DTO;
using Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Controller.Controllers;


[ApiController]
[Route("client")]
public class ClientController : ControllerBase
{
    public IConfiguration _configuration;

    public ClientController(IConfiguration config){
        _configuration = config;
    }
    [HttpPost]
    [Route("register")]
    public object registerClient([FromBody] ClientDTO client)
    {
        var cliente=Model.Client.convertDTOToModel(client);
        var id=cliente.save();
        return new{
            name=client.name,
            date_of_birth=client.date_of_birth,
            document=client.document,
            email=client.email,
            phone=client.name,
            login=client.login,
            passwd=client.passwd,
            address=client.address,
            id=id
        };
    }
    [HttpGet]
    [Route("get/{document}")]
    public object getInformations(string document)
    {
        var client=Model.Client.find(document);
        return client;
    }

    // [HttpPost]
    // [Route("get/login")]
    // public IActionResult getInformations([FromBody] ClientDTO obj){
    //     Console.WriteLine("cheguei aqui");
    //     var client = Model.Client.findLogin(obj);
    //     var result = new ObjectResult(client);
    //     Response.Headers.Add("Access-Control-Allow-Origin", "*");
    //     return result;
    // }

    [HttpPost]
    [Route("api")]
    public IActionResult tokenGenerate([FromBody] ClientDTO login){
        if(login != null && login.login != null && login.passwd != null){
            var user = Model.Client.findByUser(login.login, login.passwd);
            if(user != null){
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", user.id.ToString()),
                    new Claim("UserName", user.name.ToString()),
                    new Claim("Email", user.email.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
        else
        {
            return BadRequest("Empty credentials");
        }
    }
}