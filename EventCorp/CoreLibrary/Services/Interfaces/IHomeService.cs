using CoreLibrary.Models.ViewModels;

namespace CoreLibrary.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomeIndexViewModel> GetHomeIndexViewModelAsync();
    }
}
