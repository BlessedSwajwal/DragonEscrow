using Application.Common.Services;
using Application.Orders;
using Application.Users;
using Domain.Order.ValueObjects;
using Domain.User;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/Admin")]
public class AdminController(IJwtGenerator jwtGenerator, IUnitOfWork uow) : ControllerBase
{
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAdmin(LoginRequest request)
    {
        if (!request.Email.Equals(Admin.AdminInstance.Email, StringComparison.OrdinalIgnoreCase) || !request.Password.Equals(Admin.AdminInstance.Password, StringComparison.OrdinalIgnoreCase))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }

        var token = jwtGenerator.GenerateJwt(Admin.AdminInstance);
        var userResponse = new UserResponse(Admin.AdminInstance.Id.Value, Admin.AdminInstance.FirstName, Admin.AdminInstance.LastName, Admin.AdminInstance.Email, Admin.AdminInstance.MobileNo, Admin.AdminInstance.UserType.ToString(), token);
        return Ok(userResponse);
    }

    [HttpGet("CompletedOrders")]
    public async Task<IActionResult> GetCompletedOrders()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }
        var completedOrders = await uow.OrderRepository.GetCompletedOrders();
        return Ok(completedOrders.Adapt<List<AllOrderResponse>>());
    }

    [HttpGet("DisputedOrders")]
    public async Task<IActionResult> GetDisputedOrders()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }
        var disputedOrders = await uow.OrderRepository.GetDisputedOrders();
        return Ok(disputedOrders.Adapt<List<AllOrderResponse>>());
    }

    [HttpGet("PaidOrders")]
    public async Task<IActionResult> GetPaidOrders()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }
        var disputedOrders = await uow.OrderRepository.GetPaidOrders();
        return Ok(disputedOrders.Adapt<List<AllOrderResponse>>());
    }

    [HttpGet("MonthlyTransaction")]
    public async Task<IActionResult> GetMonthlyTransaction()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }

        return Ok(await uow.OrderRepository.GetMonthlyTransactions());
    }

    [HttpGet("PreviousMonthTransaction")]
    public async Task<IActionResult> PreviousMonthTransaction()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }

        return Ok(await uow.OrderRepository.GetPreviousMonthTransaction());
    }

    [HttpGet("GetOverAllDetails")]
    public async Task<IActionResult> GetOverAllDetails()
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }

        return Ok(await uow.OrderRepository.GetOverAllDetails());
    }

    [HttpGet("MarkOrderPaid")]
    public async Task<IActionResult> MarkOrderPaid([FromQuery] Guid orderId)
    {
        var role = User.FindFirstValue("UserType")!;
        if (!role.Equals(UserType.ADMIN.ToString()))
        {
            return Problem(detail: "Invalid Admin Credentials.");
        }

        await uow.OrderRepository.MarkOrderPaid(OrderId.Create(orderId));
        return Ok("Done");
    }
}
