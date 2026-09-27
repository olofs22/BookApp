function getApiUrl(): string {
  const hostname = window.location.hostname;
  if (hostname === 'localhost') {
    return 'http://localhost:5199/api';
  }
  return 'https://bookappbackend-eda3cga0fkg5a0hj.swedencentral-01.azurewebsites.net/api';
}

export const environment = {
  apiUrl: getApiUrl()
};