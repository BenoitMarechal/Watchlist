﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Watchlist.Data;
using Watchlist.Models;

namespace Watchlist.Controllers


{


    public class ListeFilmsController : Controller
    {
        private readonly ApplicationDbContext _contexte;
        private readonly UserManager<Utilisateur> _gestionnaire;

        public ListeFilmsController(ApplicationDbContext contexte,
           UserManager<Utilisateur> gestionnaire)
        {
            _contexte = contexte;
            _gestionnaire = gestionnaire;
        }
        public async Task<IActionResult> Index()
        {
            var id = await RecupererIdUtilisateurCourant();
            var filmsUtilisateur = _contexte.FilmsUtilisateur.Where(x => x.IdUtilisateur == id);
            var modele = filmsUtilisateur.Select(x => new ModeleVueFilm
            {
                IdFilm = x.IdFilm,
                Titre = x.Film.Titre,
                Annee = x.Film.Annee,
                Vu = x.Vu,
                PresentDansListe = true,
                Note = x.Note
            }).ToList();

            return View(modele);
        }

        private Task<Utilisateur> GetCurrentUserAsync() =>
           _gestionnaire.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<string> RecupererIdUtilisateurCourant()
        {
            Utilisateur utilisateur = await GetCurrentUserAsync();
            return utilisateur?.Id;
        }
    }
}
