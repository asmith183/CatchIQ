import { Routes, Route } from 'react-router-dom'
import ProtectedRoute from './auth/ProtectedRoute'
import AppLayout from './components/AppLayout'
import Register from './pages/Register'
import Login from './pages/Login'
import Dashboard from './pages/Dashboard'
import History from './pages/History'
import Insights from './pages/Insights'
import LogCatch from './pages/LogCatch'
import MapPage from './pages/MapPage'
import Spots from './pages/Spots'

function App() {
  return (
    <Routes>
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/" element={<Dashboard />} />
          <Route path="/history" element={<History />} />
          <Route path="/insights" element={<Insights />} />
          <Route path="/logCatch" element={<LogCatch />} />
          <Route path="/map" element={<MapPage />} />
          <Route path="/spots" element={<Spots />} />
        </Route>
      </Route>
    </Routes>
  )
}

export default App
