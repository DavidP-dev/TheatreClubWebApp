module TheatreClubWebApp.Client.AuthenticationService

open Fable.SimpleJson
open TheatreClubWebApp.Shared.Domain
open FsToolkit.ErrorHandling


let tokenKey = "user-token"
let tryGetUserToken () =
    Browser.WebStorage.localStorage.getItem tokenKey
    |> Result.requireNotNull "invalid json"
    |> Result.bind Json.tryParseAs<UserTokenInfo>
    |> function
        | Ok token -> Some token
        | _ -> None

let saveUserToken (token: UserTokenInfo) =
    Browser.WebStorage.localStorage.setItem(tokenKey, (SimpleJson.stringify token))
