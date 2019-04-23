using AutoMapper;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fullbar.Olympus.MVC.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Login, LoginViewModel>();
            Mapper.CreateMap<UsuarioCadastro, UsuarioCadastroViewModel>();
            Mapper.CreateMap<UsuarioEditar, UsuarioEditarViewModel>();
            Mapper.CreateMap<TreinamentoHome, TreinamentoHomeViewModel>();
            Mapper.CreateMap<TreinamentoDetalhe, TreinamentoDetalheViewModel>();
            Mapper.CreateMap<TreinamentoInscricao, TreinamentoInscricaoViewModel>();
            Mapper.CreateMap<TreinamentoInscricaoResultado, TreinamentoInscricaoResultadoViewModel>();
            Mapper.CreateMap<FaleConosco, FaleConoscoViewModel>();
            Mapper.CreateMap<TreinamentoPrePergunta, TreinamentoPrePerguntaViewModel>();
        }
    }
}