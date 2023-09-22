import { configureStore, combineReducers } from '@reduxjs/toolkit';

import accountSlices from './controllers/Account/AccountSlices';

const rootReducer = combineReducers({
  account: accountSlices,
  // ...
});
const store = configureStore({
  reducer: rootReducer,
});
export default store;
