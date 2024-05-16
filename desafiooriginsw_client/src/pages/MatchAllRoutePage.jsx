import NavigateButton from "../components/NavigateButton";

export function MatchAllRoute() {
    return (
        <div>
            <h2>La pagina solicitada no existe</h2>
            <NavigateButton value={"/home"} text={"Salir"} color={"red"}></NavigateButton>
        </div>
    );
}
