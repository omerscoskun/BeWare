import { Injectable } from '@angular/core';

const STORAGE_KEY = 'beware.deviceId';

@Injectable({ providedIn: 'root' })
export class DeviceIdService {
  readonly deviceId = this.getOrCreate();

  private getOrCreate(): string {
    let id = localStorage.getItem(STORAGE_KEY);
    if (!id) {
      id = crypto.randomUUID();
      localStorage.setItem(STORAGE_KEY, id);
    }
    return id;
  }
}
