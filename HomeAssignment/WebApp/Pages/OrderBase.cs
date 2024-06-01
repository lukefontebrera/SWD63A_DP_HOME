using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Models;
using System.Security.Claims;
using WebApp.Services;

namespace WebApp.Pages
{
    public class OrderBase : ComponentBase
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        [Inject]
        public IMovieService MovieService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<OrderDTO> Orders { get; set; }

        public IEnumerable<MovieDTO> Movies { get; set; }

        public string UserEmail { get; set; }
    }
}
