import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  const requiresAuth = req.headers.has('Requires-Auth');

  if (token && requiresAuth) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  req = req.clone({
    headers: req.headers.delete('Requires-Auth')
  });

  return next(req);
};
