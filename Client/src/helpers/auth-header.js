export function authHeader() {
    var userId = localStorage.getItem('userId');

    if (!userId) {
        return {};
    }

    var token = localStorage.getItem('token');

    return { 'Authorization': 'Bearer ' + token };
}