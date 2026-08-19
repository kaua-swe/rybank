using rybank.Dto.Pix;
using rybank.Enums;
using rybank.Models;

namespace rybank.Interfaces
{
    public interface IPixService
    {
        Task<PixModel?> FindPixById(Guid usuarioId);
        ChavePixEnum VerificarTipoChave(string chave);
        Task<object> CreatePix(string email, string tipoChave, string valorChave);
        Task<List<PixResponseDto>> Consultar(string email);
        Task<object> Deletar(string email, string tipoChave);
        Task<PixModel?> FindChave(string chave);
    }
}