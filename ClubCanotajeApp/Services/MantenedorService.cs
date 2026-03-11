using ClubCanotajeAPI.Models.Entities.Catalogos;
using ClubCanotajeAPI.Repositories.MantenedorRepository;
using ClubCanotajeAPI.Models.Dtos.Common;

namespace ClubCanotajeAPI.Services
{
    public class MantenedorService
    {
        private readonly MantenedorRepository _repo;

        public MantenedorService(MantenedorRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<EstadoCanoa>>> GetEstadoCanoasAsync()
        {
            var estados = await _repo.GetEstadoCanoasAsync();
            return ApiResponse<List<EstadoCanoa>>.Ok(estados);
        }

        public async Task<ApiResponse<List<TipoCanoa>>> GetTipoCanoasAsync()
        {
            var tipos = await _repo.GetTipoCanoasAsync();
            return ApiResponse<List<TipoCanoa>>.Ok(tipos);
        }

        public async Task<ApiResponse<List<MotivoCancelacion>>> GetMotivoCancelacionAsync()
        {
            var motivos = await _repo.GetMotivoCancelacionAsync();
            return ApiResponse<List<MotivoCancelacion>>.Ok(motivos);
        }

        public async Task<ApiResponse<List<RolEnSalida>>> GetRolEnSalidasAsync()
        {
            var roles = await _repo.GetRolEnSalidasAsync();
            return ApiResponse<List<RolEnSalida>>.Ok(roles);
        }

        public async Task<ApiResponse<List<TipoMembresia>>> GetTipoMembresiasAsync()
        {
            var tipos = await _repo.GetTipoMembresiasAsync();
            return ApiResponse<List<TipoMembresia>>.Ok(tipos);
        }


    }
}