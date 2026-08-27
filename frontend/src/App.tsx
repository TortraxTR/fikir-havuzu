import {BrowserRouter as Router, Route, Routes} from 'react-router-dom'
import LoginPage from './pages/LoginPage'
import LandingPage from './pages/LandingPage'

function App() {
  return (
  <Router>
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/landing" element={<LandingPage />} />
    </Routes>
  </Router>
  );
}

export default App
