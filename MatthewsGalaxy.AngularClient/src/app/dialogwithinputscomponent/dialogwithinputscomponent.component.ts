import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogData {
  title: string;
  inputs: Array<{ label: string, controlName: string, type: string }>;
}

@Component({
  selector: 'app-dialogwithinputscomponent',
  templateUrl: './dialogwithinputscomponent.component.html',
  styleUrls: ['./dialogwithinputscomponent.component.css']
})
export class DialogwithinputscomponentComponent implements OnInit {
  title: string;
  formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<DialogwithinputscomponentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.formGroup = this.fb.group({});
    this.title = this.data?.title ?? 'Default Title';
    this.createFormControls();
  }

  ngOnInit(): void {}

  private createFormControls(): void {
    if (this.data?.inputs) {
      this.data.inputs.forEach(input => {
        this.formGroup.addControl(input.controlName, this.fb.control('', Validators.required));
      });
    }
    this.formGroup.addControl('content', this.fb.control('', Validators.required));
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.dialogRef.close(this.formGroup.value);
    }
  }

  viewHtml(): void {
    const markdownContent = this.formGroup.get('content')?.value;
    const htmlContent = this.convertMarkdownToHtml(markdownContent);
  }

  private convertMarkdownToHtml(markdown: string): string {
    // Implement Markdown to HTML conversion logic here (e.g., using a library like Showdown)
    return markdown; // Placeholder, actual conversion logic needed
  }
}
