import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconEdit3, IconArrowUp, IconArrowDown, IconMenu, IconDelete, IconSave,
  IconSearch, IconPlusSquare,
  IconMessageSquare, IconX, IconSend, IconThumbsUp, IconThumbsDown} from 'angular-feather';

const icons = [
  IconEdit3,
  IconArrowUp,
  IconArrowDown,
  IconMenu,
  IconDelete,
  IconPlusSquare,
  IconSave,
  IconSearch,
  IconMessageSquare,
  IconSend,
  IconX,
  IconThumbsDown,
  IconThumbsUp
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports: icons
})
export class IconsModule { }
