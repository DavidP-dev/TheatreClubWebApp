module TheatreClubWebApp.Client.View

open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open Router
open Elmish

type private Msg =
    | UrlChanged of Page

type private State = {
    Page : Page
}

let private init () =
    let nextPage = Router.currentPath() |> Page.parseFromUrlSegments
    { Page = nextPage }, Cmd.navigatePage nextPage

let private update (msg:Msg) (state:State) : State * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { state with Page = page }, Cmd.none

[<ReactComponent>]
let AppView () =
    let state,dispatch = React.useElmish(init, update)


    let navigation =
        Daisy.navbar [
            prop.className "mb-2 shadow-lg bg-neutral text-neutral-content rounded-box"
            prop.children [
                Daisy.navbarStart []
                Daisy.navbarCenter [
                    Daisy.button.button [
                        prop.text "Úvod"
                        prop.onClick (fun _ -> Page.Index |> Router.navigatePage)
                    ]
                    Daisy.button.button [
                         prop.text "Členové klubu"
                         prop.onClick (fun _ -> Page.Members |> Router.navigatePage)
                    ]
                    Daisy.button.button [
                         prop.text "Divadelní představení"
                         prop.onClick (fun _ -> Page.Performances |> Router.navigatePage)
                    ]
                    Daisy.button.button [
                         prop.text "Rezervace"

                    ]
                ]
                Daisy.navbarEnd [
                ]
            ]
        ]


    let render =
        match state.Page with
        | Page.Index -> Pages.Index.IndexView ()
        | Page.Members -> Pages.Members.MembersView ()
        | Page.Performances -> Pages.Performances.PerformancesView ()
        | Page.Reservations -> Pages.Reservations.ReservationsView ()
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children [ navigation; render ]
    ]