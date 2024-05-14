export const storage = {
    get(key) {
        const value = localStorage.getItem(key);
        if (!value) {
            return null;
        }
        return JSON.parse(value);
    },
    set(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    remove(key) {
        localStorage.removeItem(key);
    },
    clear() {
        localStorage.clear();
    },
    getcookie(cookiename) {
        var cookiestring = document.cookie;
        var cookiearray = cookiestring.split(';');
        for (var i = 0; i < cookiearray.length; ++i) {
            if (cookiearray[i].trim().match('^' + cookiename + '=')) {
                return cookiearray[i].replace(`${cookiename}=`, '').trim();
            }
        } return null;
    }
};