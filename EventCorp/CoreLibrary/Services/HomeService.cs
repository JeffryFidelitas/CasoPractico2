using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Data;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class HomeService : IHomeService
    {
        private readonly EventCorpContext _context;

        public HomeService(EventCorpContext context)
        {
            _context = context;
        }

        public async Task<HomeIndexViewModel> GetHomeIndexViewModelAsync()
        {
            var totalEvents = await _context.Eventos.CountAsync();

            var totalActiveUsers = await _context.Usuarios
                .CountAsync(u => u.Rol == Auth.RolesEnum.Usuario);

            var totalAttendeesThisMonth = await _context.Inscripciones
                .CountAsync(i => i.FechaInscripcion.Month == DateTime.Now.Month &&
                                 i.FechaInscripcion.Year == DateTime.Now.Year);

            var top5PopularEvents = await _context.Eventos
                .OrderByDescending(e => e.Inscripciones.Count)
                .Take(5)
                .Select(e => new HomeIndexViewModel.EventPopularity
                {
                    EventName = e.Titulo,
                    AttendeeCount = e.Inscripciones.Count
                })
                .ToListAsync();

            return new HomeIndexViewModel
            {
                TotalEvents = totalEvents,
                TotalActiveUsers = totalActiveUsers,
                TotalAttendeesThisMonth = totalAttendeesThisMonth,
                Top5PopularEvents = top5PopularEvents
            };
        }

    }
}
