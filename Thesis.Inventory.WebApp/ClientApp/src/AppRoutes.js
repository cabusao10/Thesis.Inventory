import Archives from "./components/Archives";
import { Category } from "./components/Category";
import { Counter } from "./components/Counter";
import { Dashboard } from "./components/Dashboard";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Inventory } from "./components/Inventory";
import { Login } from "./components/Login";
import { Main } from "./components/Main";
import { Message } from "./components/Message";
import { Orders } from "./components/Orders";
import { PrintInventory } from "./components/PrintInventory";
import { Register } from "./components/Register";
import { UOM } from "./components/UOM";
import Users from "./components/Users";
import { Validation } from "./components/Validate";
import { Welcome } from "./components/Welcome";

const AppRoutes = [
  {
    index: true,
    element: <Welcome />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  }
  ,
  {
    path: '/login',
    element: <Login />
  }
  ,
  {
    path: '/register',
    element: <Register />
  }
  ,
  {
    path: '/dashboard',
    element: <Dashboard />
  }
  ,
  {
    path: '/orders',
    element: <Orders />
  }
  ,
  {
    path: '/inventory',
    element: <Inventory />
  }
  ,
  {
    path: '/archives',
    element: <Archives />
  }
  ,
  {
    path: '/users',
    element: <Users />
  }
  ,
  {
    path: '/chat',
    element: <Message />
  }
  ,
  {
    path: '/validate',
    element: <Validation />
  }
  ,
  {
    path: '/print',
    element: <PrintInventory />
  }
  ,
  {
    path: '/categories',
    element: <Category />
  }
  ,
  {
    path: '/uom',
    element: <UOM />
  }
];

export default AppRoutes;
