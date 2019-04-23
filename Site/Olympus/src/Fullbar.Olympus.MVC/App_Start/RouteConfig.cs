using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fullbar.Olympus.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
                        name: "CadastroEditarEnEs",
                        url: Resource.PaginaEditar + "/{action}/{id}",
                        defaults: new { controller = "Editar", action = "Index", id = UrlParameter.Optional }
                   );

            routes.MapRoute(
               name: "CodigoEticaBolsasEstudoEsEn",
               url: Resource.PaginaBolsaDeEstudo + "/{action}/{id}",
               defaults: new { controller = "BolsasEstudo", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
               name: "CodigoEticaEsEn",
               url: Resource.PaginaCodigoCondutoMenu1 + "/{action}/{id}",
               defaults: new { controller = "CodigoEtica", action = "Index", id = UrlParameter.Optional }
           );



            routes.MapRoute(
                             name: "ContatoCadastroEnEs",
                             url: Resource.PaginaContato + "/" + Resource.PaginaContatoAction + "/{id}",
                             defaults: new { controller = "Contato", action = "Cadastrar", id = UrlParameter.Optional }
                        );



            routes.MapRoute(
              name: "ContatoEnEs",
              url: Resource.PaginaContato + "/{action}/{id}",
              defaults: new { controller = "Contato", action = "Index", id = UrlParameter.Optional }
          );



            routes.MapRoute(
                              name: "TreinamentoListaEsEn",
                              url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoActionCategoria + "/{id}",
                              defaults: new { controller = "Treinamento", action = "Categoria", id = UrlParameter.Optional }
                         );


            routes.MapRoute(
                               name: "TreinamentoMeusTreinamentosEnEs",
                               url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoActionMeusTreinamentos + "/{id}",
                               defaults: new { controller = "Treinamento", action = "MeusTreinamentos", id = UrlParameter.Optional }
                          );




            routes.MapRoute(
                              name: "TreinamentoResultadoEnEs",
                              url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoActionResultado + "/{id}",
                              defaults: new { controller = "Treinamento", action = "Resultado", id = UrlParameter.Optional }
                         );

            routes.MapRoute(
                               name: "TreinamentoCadastrarInscricaoEnEs",
                               url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoActionCadastrarInscricao + "/{id}",
                               defaults: new { controller = "Treinamento", action = "CadastrarInscricao", id = UrlParameter.Optional }
                          );




            routes.MapRoute(
                                name: "TreinamentoInscricaoEnEs",
                                url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoActionInscricao + "/{id}",
                                defaults: new { controller = "Treinamento", action = "Inscricao", id = UrlParameter.Optional }
                           );


            routes.MapRoute(
                                name: "TreinamentoDetalheEnEs",
                                url: Resource.PaginaTreinamentoController + "/" + Resource.PaginaTreinamentoDetalhe + "/{id}",
                                defaults: new { controller = "Treinamento", action = "Detalhes", id = UrlParameter.Optional }
                           );


            routes.MapRoute(
                         name: "TreinamentoEsEn",
                         url: Resource.PaginaTreinamentoController + "/{action}/{id}",
                         defaults: new { controller = "Treinamento", action = "Index", id = UrlParameter.Optional }
                    );


            routes.MapRoute(
                                name: "CadastroEnEs",
                                url: Resource.PaginaCadastro + "/{action}/{id}",
                                defaults: new { controller = "Cadastro", action = "Index", id = UrlParameter.Optional }
                           );
            
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home_", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}