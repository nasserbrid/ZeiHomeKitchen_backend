using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZeiHomeKitchen_backend.Models;

namespace ZeiHomeKitchen_backend.Repositories;

public class UtilisateurRepository : IUtilisateurRepository
{
    

    private readonly UserManager<Utilisateur> _userManager;

    public UtilisateurRepository(ZeiHomeKitchenContext zeiHomeKitchenContext, UserManager<Utilisateur> userManager)
    {
       
        _userManager = userManager;
    }

    public async Task<Utilisateur?> GetUtilisateurById(int id)
    {
      

        return await _userManager.FindByIdAsync(id.ToString());
    }


    public async Task<Utilisateur?> GetUtilisateurByUsername(string username)
    {


        return await _userManager.FindByNameAsync(username);
    }

    public async Task<Utilisateur?> GetUtilisateurByEmail(string email)
    {


        return await _userManager.FindByEmailAsync(email);
    }
}
