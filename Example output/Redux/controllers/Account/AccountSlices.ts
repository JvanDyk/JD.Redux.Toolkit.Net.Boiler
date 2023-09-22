const accountGetPersonalSlice = createSlice({
  name: 'accountGetPersonal',
  initialState: { accountGetPersonalData: any as AccountGetPersonalResponse, status: 'idle', error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAccountGetPersonal.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchAccountGetPersonal.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.accountGetPersonalData = action.payload;
      })
      .addCase(fetchAccountGetPersonal.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  },
});

export default accountGetPersonalSlice.reducer;

const accountGetAddressSlice = createSlice({
  name: 'accountGetAddress',
  initialState: { accountGetAddressData: any as AccountGetAddressResponse, status: 'idle', error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAccountGetAddress.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchAccountGetAddress.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.accountGetAddressData = action.payload;
      })
      .addCase(fetchAccountGetAddress.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  },
});

export default accountGetAddressSlice.reducer;

const accountDeleteAccountSlice = createSlice({
  name: 'accountDeleteAccount',
  initialState: { accountDeleteAccountData: any as AccountDeleteAccountResponse, status: 'idle', error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchAccountDeleteAccount.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchAccountDeleteAccount.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.accountDeleteAccountData = action.payload;
      })
      .addCase(fetchAccountDeleteAccount.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  },
});

export default accountDeleteAccountSlice.reducer;

