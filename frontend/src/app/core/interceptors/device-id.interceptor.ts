import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { DeviceIdService } from '../services/device-id.service';

/** Yalnızca kendi backend'imize giden isteklere cihaz kimliğini ekler. */
export const deviceIdInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.startsWith(environment.apiUrl)) {
    const deviceId = inject(DeviceIdService).deviceId;
    req = req.clone({ setHeaders: { 'X-Device-Id': deviceId } });
  }
  return next(req);
};
