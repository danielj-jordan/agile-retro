import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconEdit3, IconArrowUp, IconArrowDown, IconMenu, IconDelete, IconSave,
  IconSearch, 
  IconMessageSquare, IconX, IconSend} from 'angular-feather';

const icons = [
  IconEdit3,
  IconArrowUp,
  IconArrowDown,
  IconMenu,
  IconDelete,
  IconSave,
  IconSearch,
  IconMessageSquare,
  IconSend,
  IconX
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: icons
})
export class IconsModule { }
