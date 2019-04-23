using AutoMapper;
using Fullbar.Olympus.Dominio.Entidade;
using Fullbar.Olympus.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fullbar.Olympus.MVC.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<LoginViewModel, Login>();
            Mapper.CreateMap<UsuarioCadastroViewModel, UsuarioCadastro>();
            Mapper.CreateMap<UsuarioEditarViewModel, UsuarioEditar>();
            Mapper.CreateMap<TreinamentoHomeViewModel, TreinamentoHome>();
            Mapper.CreateMap<TreinamentoDetalheViewModel, TreinamentoDetalhe>();
            Mapper.CreateMap<TreinamentoInscricaoViewModel, TreinamentoInscricao>();
            Mapper.CreateMap<TreinamentoInscricaoResultadoViewModel, TreinamentoInscricaoResultado>();
            Mapper.CreateMap<FaleConoscoViewModel, FaleConosco>();
            Mapper.CreateMap<TreinamentoPrePerguntaViewModel, TreinamentoPrePergunta>();
            
        }
    }
}