import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { ReserveComponent } from './reserve/reserve.component';
import { SignupComponent } from './signup/signup.component';

const routes: Routes = [
  { path: '', component: HomeComponent },

  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'dashboard', component: ProfileComponent },
  { path: 'reserve', component: ReserveComponent },



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
