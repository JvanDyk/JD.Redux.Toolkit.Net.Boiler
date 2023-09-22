export const fetchAccountGetPersonal = createAsyncThunk(
  'account/fetchAccountGetPersonal',
  async (params: AccountGetPersonalParams): Promise<any> => {
    const response = await fetch('/api/Account/GetPersonal', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(params),
    });
    const data: AccountGetPersonalResponse = await response.json();
    return data;
  }
);

export const fetchAccountGetAddress = createAsyncThunk(
  'account/fetchAccountGetAddress',
  async (params: AccountGetAddressParams): Promise<any> => {
    const response = await fetch('/api/Account/GetAddress', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(params),
    });
    const data: AccountGetAddressResponse = await response.json();
    return data;
  }
);

export const fetchAccountDeleteAccount = createAsyncThunk(
  'account/fetchAccountDeleteAccount',
  async (params: AccountDeleteAccountParams): Promise<any> => {
    const response = await fetch('/api/Account/DeleteAccount', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(params),
    });
    const data: AccountDeleteAccountResponse = await response.json();
    return data;
  }
);

