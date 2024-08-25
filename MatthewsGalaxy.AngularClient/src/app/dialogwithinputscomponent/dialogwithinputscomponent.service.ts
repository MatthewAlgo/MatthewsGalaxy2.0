// input-dialog.service.ts
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DialogwithinputscomponentComponent, DialogData } from './dialogwithinputscomponent.component';

@Injectable({
  providedIn: 'root'
})
export class InputDialogService {
  dialogRef: MatDialogRef<DialogwithinputscomponentComponent> | undefined;

  constructor(private dialog: MatDialog) {}

  openInputDialog(data: any): MatDialogRef<DialogwithinputscomponentComponent> {
    const dialogData: DialogData = {
      title: 'My Dialog Title',
      inputs: [
        { label: 'Title', controlName: 'title', type: 'text' },
      ]
    };

    this.dialogRef = this.dialog.open(DialogwithinputscomponentComponent, {
      data: dialogData,
    });

    return this.dialogRef;
  }
}
