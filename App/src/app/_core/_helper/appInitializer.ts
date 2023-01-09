import { Authv2Service } from 'src/app/_core/_service/authv2.service';

export function appInitializer(authService: Authv2Service) {
    return () =>
      new Promise((resolve, reject) => {
            // authService.refreshToken().subscribe().add(resolve);
        });
}
