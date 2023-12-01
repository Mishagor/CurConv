import logo from './logo.svg';
import "./App.scss"
import { ContentPage } from "./components/ContentPage";

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo" />
                <ContentPage />
            </header>
        </div>
    );
}

export default App;