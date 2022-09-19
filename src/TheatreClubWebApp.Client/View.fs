module TheatreClubWebApp.Client.View

open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open Router
open Elmish
open TheatreClubWebApp.Shared.Domain

type private Msg =
    | UrlChanged of Page

type private State = {
    Page : Page
    UserTokenInfo : UserTokenInfo option
}

let private init userTokenInfo () =
    let nextPage =
        match userTokenInfo with
        | Some _ -> Router.currentPath() |> Page.parseFromUrlSegments
        | None -> Page.Login
    { Page = nextPage; UserTokenInfo = userTokenInfo }, Cmd.navigatePage nextPage

let private update (msg:Msg) (state:State) : State * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { state with Page = page }, Cmd.none

[<ReactComponent>]
let AppView () =
    let userTokenInfo  = AuthenticationService.tryGetUserToken ()
    let state,dispatch = React.useElmish(init userTokenInfo, update)


    let navigation =
        Daisy.navbar [
            prop.className "mb-4 shadow-sm bg-base-200 text-neutral-content rounded-box"
            prop.children [
                Daisy.navbarStart []
                Daisy.navbarCenter [
                    Daisy.button.button [
                         button.outline
                         button.primary
                         prop.text "Členové klubu"
                         prop.onClick (fun _ -> Page.Members |> Router.navigatePage)
                    ]
                    Daisy.button.button [
                         button.outline
                         button.primary
                         prop.text "Divadelní představení"
                         prop.onClick (fun _ -> Page.Performances |> Router.navigatePage)
                    ]
                    Daisy.button.button [
                         button.outline
                         button.primary
                         prop.text "Rezervace"
                         prop.onClick (fun _ -> Page.Reservations |> Router.navigatePage)

                    ]
                ]
                Daisy.navbarEnd [
                ]
            ]
        ]

    let content =
        match state.Page with
        | Page.Login -> Pages.Login.LoginView()
        | _ ->
            let render =
                match state.Page with
                | Page.Index -> Pages.Index.IndexView ()
                | Page.Members -> Pages.Members.MembersView ()
                | Page.Performances -> Pages.Performances.PerformancesView ()
                | Page.Reservations -> Pages.Reservations.ReservationsView ()
                | Page.AddMember -> Pages.AddMember.AddMemberView ()
                | Page.AddPerformance -> Pages.AddPerformance.AddPerformanceView ()
                | Page.AddReservation -> Pages.AddReservation.AddReservationView ()
                | Page.EditMember i -> Pages.EditMember.EditMemberView i
                | Page.EditPerformance i -> Pages.EditPerformance.EditPerformanceView i
                | Page.EditReservation i -> Pages.EditReservation.EditReservationView i
                | Page.ArchiveOfPerformances -> Pages.ArchiveOfPerformances.ArchiveOfPerformancesView ()
                | Page.ArchiveOfReservations -> Pages.ArchiveOfReservations.ArchiveOfReservationsView ()
            React.fragment [ navigation; render ]


    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children content
    ]