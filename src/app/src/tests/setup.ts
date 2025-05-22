import { expect, afterEach, vi, describe, it, beforeEach } from 'vitest';
import * as matchers from '@testing-library/jest-dom/matchers';
import '@testing-library/jest-dom';

// Extend Vitest's expect with jest-dom matchers
expect.extend(matchers);

// Make vitest functions available globally
Object.assign(global, {
  describe,
  it,
  expect,
  vi,
  beforeEach,
  afterEach
});

// This will make TypeScript recognize these globals
declare global {
  const describe: typeof import('vitest')['describe'];
  const it: typeof import('vitest')['it'];
  const expect: typeof import('vitest')['expect'];
  const vi: typeof import('vitest')['vi'];
  const beforeEach: typeof import('vitest')['beforeEach'];
  const afterEach: typeof import('vitest')['afterEach'];
}
