module TheatreClubWebApp.Client.Pages.Login


open Elmish
open Feliz
open Feliz.UseElmish
open Feliz.DaisyUI
open TheatreClubWebApp.Client
open TheatreClubWebApp.Client.Router
open TheatreClubWebApp.Shared.API
open TheatreClubWebApp.Client.Server
open TheatreClubWebApp.Shared.Domain

type Model = {
    Password : string
    Error : string option
    }

type Message =
    | PasswordChanged of string
    | FormSubmitted
    | UserAuthenticated of UserTokenInfo
    | LoginError of exn

let update msg (state: Model) =
    match msg with
    | PasswordChanged p -> { state with Password = p }, Cmd.none
    | FormSubmitted ->
        {state with Error = None}, Cmd.OfAsync.either servicePass.AddPassword state.Password UserAuthenticated LoginError
    | UserAuthenticated userTokenInfo ->
        AuthenticationService.saveUserToken userTokenInfo
        state, Page.Index |> Cmd.navigatePage
    | LoginError ex  -> {state with Error = Some "Chyba přihlášení"}, Cmd.none
let init () =
    {
        Password = ""
        Error = None
    },Cmd.none

[<ReactComponent>]
let LoginView () =

    let state,dispatch = React.useElmish(init, update, [| |])

    Html.div [
        prop.className "flex flex-col gap-4 mx-auto"
        prop.children [
            Daisy.alert [
                alert.info
                prop.className "justify-center bg-base-100"
                prop.text "Ahoj Romčo, zadej svůj kód, aby ses dostala k přísně střeženým údajům členů divadelního klubu."
            ]
            match state.Error with
            | Some err ->
                Daisy.alert [
                    alert.error
                    prop.className "justify-center bg-base-100"
                    prop.text err
                ]
            | None -> Html.none

            Html.div [
                prop.className "flex justify-center"

                prop.children [
                    Daisy.formControl [
                        prop.className "flex gap-4"
                        prop.children [
                            Daisy.label [Daisy.labelText "Heslo hýr:"]
                            Daisy.input [
                                prop.type'.password
                                input.bordered
                                prop.placeholder "Heslo"
                                prop.value state.Password
                                prop.onChange (fun p -> PasswordChanged p |> dispatch)
                            ]

                            Daisy.button.submit [
                                button.outline
                                button.primary
                                button.lg
                                prop.value "Potvrď heslo"
                                prop.onClick (fun ev ->
                                    ev.preventDefault()
                                    FormSubmitted |> dispatch
                                )
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

